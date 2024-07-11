using BlossomApi.DB;
using BlossomApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlossomApi.Dtos;
using Microsoft.EntityFrameworkCore;

namespace BlossomApi.Services
{
    public class CategoryService
    {
        private readonly BlossomContext _context;

        public CategoryService(BlossomContext context)
        {
            _context = context;
        }

        public async Task<CategoryNode> GetCategoryTreeAsync(string categoryName)
        {
            var allCategories = await _context.Categories.ToListAsync();

            var rootCategory = allCategories.FirstOrDefault(c => c.Name.ToLower() == categoryName.ToLower());
            if (rootCategory == null)
            {
                return null;
            }

            var categoryTree = BuildCategoryTree(rootCategory, allCategories);
            return categoryTree;
        }

        public List<string> GetAllCategoryNames(CategoryNode root)
        {
            var names = new List<string>();
            TraverseCategoryTree(root, names);
            return names;
        }

        private void TraverseCategoryTree(CategoryNode node, List<string> names)
        {
            if (node == null) return;

            names.Add(node.Name);

            foreach (var child in node.Children)
            {
                TraverseCategoryTree(child, names);
            }
        }

        private CategoryNode BuildCategoryTree(Category root, List<Category> allCategories)
        {
            var rootNode = new CategoryNode
            {
                CategoryId = root.CategoryId,
                Name = root.Name,
                Children = new List<CategoryNode>()
            };

            var childCategories = allCategories.Where(c => c.ParentCategoryId == root.CategoryId).ToList();

            foreach (var child in childCategories)
            {
                rootNode.Children.Add(BuildCategoryTree(child, allCategories));
            }

            return rootNode;
        }
    }
}