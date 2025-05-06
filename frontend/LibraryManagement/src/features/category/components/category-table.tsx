// src/features/category/components/CategoriesTable.tsx
import React from "react";
import { Table, TableBody, TableRow, TableCell } from "@/components/ui/table";
import { BookTableHeader } from "@/components/app/table-header"; // Reusing this, could be renamed to TableListHeader
import { CategoryResponse } from "@/features/category/types/type";
import { CategoryActions } from "./category-actions";

interface CategoriesTableProps {
  categories: CategoryResponse[];
  headers: string[];
  onEditCategory: (category: CategoryResponse) => void;
  onDeleteCategory: (id: string) => void;
}

export const CategoriesTable: React.FC<CategoriesTableProps> = ({
  categories,
  headers,
  onEditCategory,
  onDeleteCategory,
}) => {
  return (
    <div className="rounded-md border overflow-auto">
      <Table className="w-full">
        <BookTableHeader headers={headers} /> {/* Reusing */}
        <TableBody>
          {categories.length === 0 && (
            <TableRow>
              <TableCell colSpan={headers.length} className="text-center">
                No categories found.
              </TableCell>
            </TableRow>
          )}
          {categories.map((cat) => (
            <TableRow key={cat.id} className="hover:bg-gray-50">
              <TableCell className="font-medium whitespace-nowrap overflow-hidden text-ellipsis">
                {cat.name}
              </TableCell>
              <TableCell className="w-36">
                <CategoryActions
                  category={cat}
                  onEdit={onEditCategory}
                  onDelete={onDeleteCategory}
                />
              </TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </div>
  );
};
