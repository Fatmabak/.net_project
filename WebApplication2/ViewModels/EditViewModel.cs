namespace WebApplication2.ViewModels
{
	public class EditViewModel : CreateViewModel
	{
		public int Id { get; set; }
		public string ExistingImagePath { get; set; }
        public string Categorie { get; set; }
        public string Marque { get; set; }
    }
}
