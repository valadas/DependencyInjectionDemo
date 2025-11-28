# Simple C# Class Dependency Graph Generator
param(
    [string]$ProjectPath = ".",
    [string]$OutputFile = "class-dependencies.dot"
)

$classes = @{}
$dependencies = @{}
$builtInTypes = @('string', 'int', 'bool', 'double', 'float', 'decimal', 'long', 'short', 'byte', 'char', 'object', 'void', 'var')

# Find all C# files
Get-ChildItem -Path $ProjectPath -Filter "*.cs" -Recurse | 
    Where-Object { $_.FullName -notmatch "\\obj\\" -and $_.FullName -notmatch "\\bin\\" } | 
    ForEach-Object {
        $content = Get-Content $_.FullName -Raw
        
        # Extract namespace
        $namespace = ""
        if ($content -match 'namespace\s+([\w\.]+)') {
            $namespace = $Matches[1]
        }
        
        # Extract class name
        if ($content -match 'class\s+(\w+)') {
            $className = $Matches[1]
            $fullClassName = "$namespace.$className"
            $classes[$fullClassName] = $true
            $dependencies[$fullClassName] = [System.Collections.Generic.HashSet[string]]::new()
            
            # Find constructor parameters
            if ($content -match 'public\s+' + $className + '\s*\(([^)]*)\)') {
                $params = $Matches[1]
                $params -split ',' | ForEach-Object {
                    if ($_ -match '(\w+)\s+\w+\s*$') {
                        $paramType = $Matches[1].Trim()
                        if ($paramType -notin $builtInTypes) {
                            [void]$dependencies[$fullClassName].Add($paramType)
                        }
                    }
                }
            }
            
            # Find private/public fields (e.g., private List<Product> products;)
            $fieldMatches = [regex]::Matches($content, '(private|public|protected|internal)\s+([\w<>]+)\s+(\w+)\s*[;=]')
            foreach ($match in $fieldMatches) {
                $fieldType = $match.Groups[2].Value
                # Extract generic type (e.g., List<Product> -> Product)
                if ($fieldType -match '<(\w+)>') {
                    $innerType = $Matches[1]
                    if ($innerType -notin $builtInTypes) {
                        [void]$dependencies[$fullClassName].Add($innerType)
                    }
                }
                # Also add the outer type if it's not built-in
                $baseType = $fieldType -replace '<.*>', ''
                if ($baseType -notin $builtInTypes -and $baseType -notmatch '^(List|Dictionary|IEnumerable|IList|ICollection)$') {
                    [void]$dependencies[$fullClassName].Add($baseType)
                }
            }
            
            # Find method return types
            $methodMatches = [regex]::Matches($content, '(public|private|protected|internal)\s+([\w<>]+)\s+(\w+)\s*\(')
            foreach ($match in $methodMatches) {
                $returnType = $match.Groups[2].Value
                # Extract generic type
                if ($returnType -match '<(\w+)>') {
                    $innerType = $Matches[1]
                    if ($innerType -notin $builtInTypes) {
                        [void]$dependencies[$fullClassName].Add($innerType)
                    }
                }
            }
            
            # Find properties
            $propMatches = [regex]::Matches($content, '(public|private|protected|internal)\s+([\w<>]+)\s+(\w+)\s*\{')
            foreach ($match in $propMatches) {
                $propType = $match.Groups[2].Value
                # Extract generic type
                if ($propType -match '<(\w+)>') {
                    $innerType = $Matches[1]
                    if ($innerType -notin $builtInTypes) {
                        [void]$dependencies[$fullClassName].Add($innerType)
                    }
                }
            }
            
            # Find object instantiation (e.g., new ProductRepository())
            $newMatches = [regex]::Matches($content, 'new\s+(\w+)\s*\(')
            foreach ($match in $newMatches) {
                $newType = $match.Groups[1].Value
                if ($newType -notin $builtInTypes -and $newType -ne $className) {
                    [void]$dependencies[$fullClassName].Add($newType)
                }
            }
        }
    }

# Generate DOT file
$dot = @"
digraph ClassDependencies {
    rankdir=LR;
    node [shape=box, style=rounded, fontname="Arial"];
    
"@

# Add nodes
foreach ($class in $classes.Keys) {
    $simpleName = $class.Split('.')[-1]
    $dot += "    `"$class`" [label=`"$simpleName`"];`n"
}

$dot += "`n"

# Add edges
foreach ($class in $dependencies.Keys) {
    foreach ($dep in $dependencies[$class]) {
        # Try to find full class name for dependency
        $fullDep = $classes.Keys | Where-Object { $_ -match "\.$dep$" } | Select-Object -First 1
        if ($fullDep) {
            $dot += "    `"$class`" -> `"$fullDep`";`n"
        }
    }
}

$dot += "}`n"

$dot | Out-File -FilePath $OutputFile -Encoding utf8
Write-Host "Class dependency graph written to: $OutputFile" -ForegroundColor Green
Write-Host "To visualize, use: https://dreampuf.github.io/GraphvizOnline/" -ForegroundColor Cyan
