using System.ComponentModel.DataAnnotations;

namespace WebApplication2.ViewModels
{
    public class CreateViewModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Désignation{ get; set; }
        [Required]
        [Display(Name = "Prix en dinar :")]
        public float Prix { get; set; }
        [Required]
        [Display(Name = "Quantité en unité :")]
        public int Quantite { get; set; }
        [Required]
        [Display(Name = "Image :")]
        public IFormFile ImagePath { get; set; }
		[Required]
		[StringLength(50, MinimumLength = 3)]
		public string Categorie { get; set; }

		[Required]
		[StringLength(50, MinimumLength = 3)]
		public string Marque { get; set; }
	}
}
