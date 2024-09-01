using BlossomApi.DB;
using BlossomApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BlossomApi.Dtos;

namespace BlossomApi.Services
{
    public class CategoryService
    {
        private readonly BlossomContext _context;

        public CategoryService(BlossomContext context)
        {
            _context = context;
        }

        public async Task<CategoryNode> GetCategoryTreeAsync(int categoryId)
        {
            var allCategories = await _context.Categories.ToListAsync();

            var rootCategory = allCategories.FirstOrDefault(c => c.CategoryId == categoryId);
            if (rootCategory == null)
            {
                return null;
            }

            var categoryTree = BuildCategoryTree(rootCategory, allCategories);
            return categoryTree;
        }

        public List<int> GetAllCategoryIds(CategoryNode root)
        {
            var ids = new List<int>();
            TraverseCategoryTree(root, ids);
            return ids;
        }

        public async Task<List<Category>> GetAllChildCategoriesAsync(int parentId)
        {
            var categories = await _context.Categories
                .Where(c => c.ParentCategoryId == parentId)
                .ToListAsync();

            var allChildCategories = new List<Category>();

            foreach (var category in categories)
            {
                allChildCategories.Add(category);
                allChildCategories.AddRange(await GetAllChildCategoriesAsync(category.CategoryId));
            }

            return allChildCategories;
        }

        // Updated method to handle building a forest (list of trees) from multiple root categories
        public List<CategoryNode> BuildCategoryForestFromList(List<Category> categories)
        {
            var lookup = categories.ToLookup(c => c.ParentCategoryId);

            // Find categories that don't have a parent in the list (they are roots)
            var rootCategories = categories.Where(c => !categories.Any(p => p.CategoryId == c.ParentCategoryId)).ToList();

            List<CategoryNode> categoryForest = new List<CategoryNode>();

            foreach (var root in rootCategories)
            {
                var rootNode = BuildCategoryTreeFromLookup(root, lookup);
                categoryForest.Add(rootNode);
            }

            return categoryForest;
        }

        private void TraverseCategoryTree(CategoryNode node, List<int> ids)
        {
            if (node == null) return;

            ids.Add(node.CategoryId);

            foreach (var child in node.Children)
            {
                TraverseCategoryTree(child, ids);
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

        private CategoryNode BuildCategoryTreeFromLookup(Category root, ILookup<int, Category> lookup)
        {
            var rootNode = new CategoryNode
            {
                CategoryId = root.CategoryId,
                Name = root.Name,
                Children = new List<CategoryNode>()
            };

            var childCategories = lookup[root.CategoryId];

            foreach (var child in childCategories)
            {
                rootNode.Children.Add(BuildCategoryTreeFromLookup(child, lookup));
            }

            return rootNode;
        }
    }
}
