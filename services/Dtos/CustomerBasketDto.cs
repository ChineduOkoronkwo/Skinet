using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace services.Dtos
{
    public class CustomerBasketDto
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public List<BasketItemDto> Items { get; set; }
    }
}