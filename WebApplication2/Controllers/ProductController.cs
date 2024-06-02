using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models.Repositories;
using WebApplication2.Models;
using WebApplication2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.IO; // Add this for Path.Combine

namespace WebApplication2.Controllers
{
	//[Authorize]
	public class ProductController : Controller
	{
		readonly IRepository<Product> productRepository;
		private readonly IWebHostEnvironment hostingEnvironment;

		public ProductController(IRepository<Product> ProdRepository, IWebHostEnvironment hostingEnvironment)
		{
			productRepository = ProdRepository;
			this.hostingEnvironment = hostingEnvironment;
		}

		[AllowAnonymous]
		public ActionResult Index()
		{
			var Produits = productRepository.GetAll();
			return View(Produits);
		}

		public ActionResult Details(int id)
		{
			return View(productRepository.Get(id));
		}

		public ActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(CreateViewModel model)
		{
			if (ModelState.IsValid)
			{
				string uniqueFileName = null;
				if (model.ImagePath != null)
				{
					string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
					uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImagePath.FileName;
					string filePath = Path.Combine(uploadsFolder, uniqueFileName);
					model.ImagePath.CopyTo(new FileStream(filePath, FileMode.Create));
				}
				Product newProduct = new Product
				{
					Désignation = model.Désignation,
					Prix = model.Prix,
					Quantite = model.Quantite,
					Image = uniqueFileName,
					Categorie = model.Categorie, // Add this line
					Marque = model.Marque // Add this line
				};
				productRepository.Add(newProduct);
				return RedirectToAction("details", new { id = newProduct.Id });
			}
			return View();
		}

		public ActionResult Edit(int id)
		{
			Product product = productRepository.Get(id);
			EditViewModel productEditViewModel = new EditViewModel
			{
				Id = product.Id,
				Désignation = product.Désignation,
				Prix = product.Prix,
				Quantite = product.Quantite,
				ExistingImagePath = product.Image,
				Categorie = product.Categorie, // Add this line
				Marque = product.Marque // Add this line
			};
			return View(productEditViewModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(EditViewModel model)
		{
			if (ModelState.IsValid)
			{
				Product product = productRepository.Get(model.Id);
				product.Désignation = model.Désignation;
				product.Prix = model.Prix;
				product.Quantite = model.Quantite;
				if (model.ImagePath != null)
				{
					if (model.ExistingImagePath != null)
					{
						string filePath = Path.Combine(hostingEnvironment.WebRootPath, "images", model.ExistingImagePath);
						System.IO.File.Delete(filePath);
					}
					product.Image = ProcessUploadedFile(model);
				}
				product.Categorie = model.Categorie; // Add this line
				product.Marque = model.Marque; // Add this line
				Product updatedProduct = productRepository.Update(product);
				if (updatedProduct != null)
					return RedirectToAction("Index");
				else
					return NotFound();
			}
			return View(model);
		}

		[NonAction]
		private string ProcessUploadedFile(EditViewModel model)
		{
			string uniqueFileName = null;
			if (model.ImagePath != null)
			{
				string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
				uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImagePath.FileName;
				string filePath = Path.Combine(uploadsFolder, uniqueFileName);
				using (var fileStream = new FileStream(filePath, FileMode.Create))
				{
					model.ImagePath.CopyTo(fileStream);
				}
			}
			return uniqueFileName;
		}

		public ActionResult Delete(int id)
		{
			return View(productRepository.Get(id));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(Product p)
		{
			try
			{
				productRepository.Delete(p.Id);
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		public ActionResult Search(string term)
		{
			var result = productRepository.Search(term);
			return View("Index", result);
		}

        public ActionResult Filter(string category, string marque)
        {
            var produits = productRepository.GetAll();

            if (!string.IsNullOrEmpty(category))
            {
                produits = produits.Where(p => p.Categorie == category).ToList();
            }

            if (!string.IsNullOrEmpty(marque))
            {
                produits = produits.Where(p => p.Marque == marque).ToList();
            }

            return View("Index", produits);
        }

       


    }
}
