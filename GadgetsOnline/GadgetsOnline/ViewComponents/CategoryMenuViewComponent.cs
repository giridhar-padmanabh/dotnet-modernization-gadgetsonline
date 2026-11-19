using GadgetsOnline.Services;
using Microsoft.AspNetCore.Mvc;

namespace GadgetsOnline.ViewComponents
{
    public class CategoryMenuViewComponent : ViewComponent
    {
        private readonly Inventory _inventory;

        public CategoryMenuViewComponent(Inventory inventory)
        {
            _inventory = inventory;
        }

        public IViewComponentResult Invoke()
        {
            var categories = _inventory.GetAllCategories();
            return View(categories);
        }
    }
}
