using Store.Core;
using Store.Core.Entities;
using Store.Core.Entities.Order_Aggregate;
using Store.Core.Repositories.Contracts;
using Store.Core.Services.Contracts;
using Store.Repository.Specifications.Order_Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services
{
	public class OrderService : IOrderService
	{
		private readonly IBasketRepository _basketRepository;
		private readonly IUnitOfWork _unitOfWork;

		public OrderService(IBasketRepository basketRepository, IUnitOfWork unitOfWork)
		{
			_basketRepository = basketRepository;
			_unitOfWork = unitOfWork;
		}

		public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
		{
			//1.Get Basket From Basket Repo

			var basket = await _basketRepository.GetBasketAsync(basketId);

			//2.Extracting orderItem objects from the basket
			var orderItems = new List<OrderItem>();

			if (basket == null || basket.Items.Count == 0)
			{
				throw new Exception("Basket is empty or not found.");
			}

			foreach (var item in basket.Items)
			{
				var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id) ??
					throw new Exception($"Product with ID {item.Id} not found.");


				var productOrdered = new ProductInOrderItem(product.Id, product.Name, product.PictureUrl);

				var OrderItem = new OrderItem(productOrdered, product.Price, item.Quantity);

				orderItems.Add(OrderItem);
			}
			//3.Get Delivery Method From DeliveryMethod Repo

			var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId) ??
				throw new Exception("Delivery method not found.");

			//4.Calculate SubTotal1

			var subTotal = orderItems.Sum(oi => oi.Price * oi.Quantity);

			//5.Create Order

			var order = new Order(buyerEmail, shippingAddress, deliveryMethod, orderItems, subTotal);

			//6.Add Order Locally

			await _unitOfWork.Repository<Order>().AddAsync(order);

			//7.Save Order To Database[ToDo]

			var result = await _unitOfWork.CompleteAsync();

			if (result <= 0) return null;

			return order;
		}

		public Task<IReadOnlyList<Order>> GetAllOrdersForAUserAsync(string BuyerEmail)
		{
			var orderspecs = new OrderSpecs(BuyerEmail);

			var Orders = _unitOfWork.Repository<Order>().GetAllWithSpecAsync(orderspecs);

			return Orders;
		}

		public Task<Order?> GetOrderByIdForAUserAsync(string BuyerEmail, int OrderID)
		{
			var orderspecs = new OrderSpecs(BuyerEmail, OrderID);
			var order = _unitOfWork.Repository<Order>().GetByIdWithSpecAsync(orderspecs);
			return order;
		}

	}
}
