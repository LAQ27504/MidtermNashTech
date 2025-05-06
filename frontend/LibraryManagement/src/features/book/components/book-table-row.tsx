import { TableRow, TableCell } from "@/components/ui/table";
import { Checkbox } from "@/components/ui/checkbox";
import { AvailableBadge } from "@/components/app/available-badge";
import { CategoryBadge } from "@/components/app/category-badge";
import { BookResponse } from "../types/type";

interface Props {
  book: BookResponse;
  isChecked: boolean;
  isDisabled: boolean;
  onCheck: (checked: boolean) => void;
}

export const BookTableRow: React.FC<Props> = ({
  book,
  isChecked,
  isDisabled,
  onCheck,
}) => (
  <TableRow>
    <TableCell className="w-12">
      <Checkbox
        checked={isChecked}
        disabled={isDisabled}
        onCheckedChange={(val) => onCheck(val === true)}
      />
    </TableCell>
    <TableCell>{book.name}</TableCell>
    <TableCell>{book.author}</TableCell>
    <TableCell>
      <CategoryBadge category={book.categoryName} />
    </TableCell>
    <TableCell>
      <AvailableBadge count={book.availableAmount} />
    </TableCell>
  </TableRow>
);
