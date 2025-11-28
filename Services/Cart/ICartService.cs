namespace DependencyInjectionDemo.Services.Cart
{
    /// <summary>
    /// Defines operations for managing a shopping cart, including adding products and processing checkout.
    /// </summary>
    /// <remarks>Implementations of this should ensure that products are valid and available before
    /// adding them to the cart. The checkout process should validate payment information and handle transaction
    /// completion. Thread safety and error handling may vary depending on the implementation.</remarks>
    public interface ICartService
    {
        /// <summary>
        /// Adds a product to the collection using the specified product identifier.
        /// </summary>
        /// <param name="productId">The unique identifier of the product to add. Must be a valid product ID.</param>
        void AddProduct(int productId);

        /// <summary>
        /// Processes a checkout transaction using the specified credit card number.
        /// </summary>
        /// <param name="creditardNumber">The credit card number to be used for the transaction. Must be a valid, non-empty string representing a
        /// supported credit card format.</param>
        /// <returns>true if the checkout is successful; otherwise, false.</returns>
        bool Checkout(string creditardNumber);
    }
}