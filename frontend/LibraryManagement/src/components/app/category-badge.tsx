export const CategoryBadge = ({ category }: { category: string }) => {
  return (
    <span className="inline-block rounded-full bg-purple-100 px-3 py-1 text-sm font-medium text-purple-700">
      {category}
    </span>
  );
};
