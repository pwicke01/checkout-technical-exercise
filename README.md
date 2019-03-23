# Checkout API

## Overview

An API that lets a customer add products to a basket, remove them or clear the basket altogether.

## Usage

Running the API locally, you will be taken to the Swagger document for the API which will allow you to see all the endpoints and what they do, as well as making requests.

Alternatively, to use the API in code, the simplest thing to do is to import the project Checkout.Client - as has been done in the Checkout.Client.Tests project. The idea is that this would be a NuGet package that could be imported into whichever project is aiming to use the API.

### Prerequisites to usage

Before attempting to add a product to the basket, you will first need to know what products are available. Simply call GET on the Product API to do this.

### Example:

```csharp
var products = await _checkoutClient.GetProducts();
```

Once you know which product you want to add, you can add it like this:

```csharp

var product = products.FirstOrDefault(); //For example, just the first one

await _checkoutClient.AddItemsToBasket(new AddItemsRequest
{
  ProductId = product.Id,
  Quantity = 3
});
```

To see the contents of your basket, you can then make a call to get the basket:

```csharp
var basket = await _checkoutClient.GetCustomerBasket();
```