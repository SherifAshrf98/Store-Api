﻿using System.ComponentModel.DataAnnotations;

namespace Store.APIs.DTOs
{
	public class BasketItemDto
	{
		[Required]
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		public string PictureUrl { get; set; }

		[Required]
		public string Brand { get; set; }

		[Required]
		public string Type { get; set; }

		[Required]
		[Range(0.1, double.MaxValue, ErrorMessage = "Price Can't Be Zero")]
		public decimal Price { get; set; }

		[Required]
		[Range(1, int.MaxValue, ErrorMessage = "you have to select at least one iteam")]
		public int Quantity { get; set; }
	}
}
