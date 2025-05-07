// src/features/category/pages/CategoriesPage.tsx
import React, { useState } from "react";
import { Button } from "@/components/ui/button";
import {
  CreateCategory,
  UpdateCategory,
  DeleteCategory,
  GetCategoryPaginated,
} from "@/features/category/api/categoryApi";
import {
  CategoryRequest,
  CategoryResponse,
} from "@/features/category/types/type";
import { PaginationRequest, PaginationResponse } from "@/constants/Common";
import { TableFooter } from "@/components/app/table-footer"; // Reusing this, could be TableListFooter

// Import new category components
import { CategoriesTable } from "../components/category-table";
import { CategoryDialogComponent } from "../components/category-dialog";

export default function CategoriesPage() {
  const [categories, setCategories] = useState<CategoryResponse[]>([]);
  const [isDialogOpen, setIsDialogOpen] = useState(false);
  const [editingCategory, setEditingCategory] =
    useState<CategoryResponse | null>(null);
  const [refreshKey, setRefreshKey] = useState(0); // Key to trigger re-fetch in footer

  const defaultPageSize = 10;

  const handleSaveCategory = async (formData: CategoryRequest) => {
    try {
      if (editingCategory) {
        await UpdateCategory(editingCategory.id, formData);
        // Add success toast/notification if available
      } else {
        await CreateCategory(formData);
        // Add success toast/notification if available
      }
      setIsDialogOpen(false);
      setEditingCategory(null);
      setRefreshKey((prev) => prev + 1); // Trigger a data refresh
    } catch (error) {
      console.error("Failed to save category:", error);
      // Add error toast/notification if available
      alert("Failed to save category. Please check the console for details.");
    }
  };

  const handleDeleteCategory = async (id: string) => {
    // Optional: Add a confirmation dialog before deleting
    if (window.confirm("Are you sure you want to delete this category?")) {
      try {
        await DeleteCategory(id);
        // Add success toast/notification if available
        setRefreshKey((prev) => prev + 1); // Trigger a data refresh
      } catch (error) {
        console.error("Failed to delete category:", error);
        // Add error toast/notification if available
        alert(
          "Failed to delete category. Please check the console for details."
        );
      }
    }
  };

  const openAddDialog = () => {
    setEditingCategory(null);
    setIsDialogOpen(true);
  };

  const openEditDialog = (category: CategoryResponse) => {
    setEditingCategory(category);
    setIsDialogOpen(true);
  };

  const categoryTableHeaders = ["Name", "Actions"];

  const fetchCategoriesData = async (
    pageNumber: number,
    pageSize: number
  ): Promise<PaginationResponse<CategoryResponse>> => {
    const req: PaginationRequest = { pageNumber, pageSize };
    const res = await GetCategoryPaginated(req);
    return res.data.data; // Ensure this matches expected shape for BookTableFooter
  };

  const handleDataFetchedFromFooter = (
    fetchedCategories: CategoryResponse[],
    totalItems: number // totalItems can be used if needed
  ) => {
    setCategories(fetchedCategories);
  };

  return (
    <div className="p-6 space-y-6">
      <div className="flex justify-between items-center">
        <h1 className="text-2xl font-semibold">Categories</h1>
        <Button onClick={openAddDialog}>Add Category</Button>
      </div>

      <CategoryDialogComponent
        isOpen={isDialogOpen}
        onOpenChange={setIsDialogOpen}
        editingCategory={editingCategory}
        onSubmit={handleSaveCategory}
      />

      <CategoriesTable
        categories={categories}
        headers={categoryTableHeaders}
        onEditCategory={openEditDialog}
        onDeleteCategory={handleDeleteCategory}
      />

      <TableFooter // Reusing the generic footer component
        key={refreshKey}
        fetchData={fetchCategoriesData}
        onDataFetched={handleDataFetchedFromFooter}
        defaultPageSize={defaultPageSize}
      />
    </div>
  );
}
