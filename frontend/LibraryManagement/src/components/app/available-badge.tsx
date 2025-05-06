export const AvailableBadge = ({ count }: { count: number }) => {
  const isAvailable = count > 0;

  const bgColor = isAvailable ? "bg-green-100" : "bg-red-100";
  const textColor = isAvailable ? "text-green-700" : "text-red-700";
  const dotColor = isAvailable ? "bg-green-500" : "bg-red-500";
  console.log("AvailableBadge count:", count);
  return (
    <div
      className={`inline-flex items-center space-x-2 rounded-full px-3 py-1 text-sm font-medium ${bgColor} ${textColor}`}>
      <span className={`h-2 w-2 rounded-full ${dotColor}`}></span>
      <span>{isAvailable ? `${count} available` : "Not available"}</span>
    </div>
  );
};
