type Props = {
  page: number;
  totalPages: number;
  hasPrevious: boolean;
  hasNext: boolean;
  onPageChange: (page: number) => void;
};

function NotePagination({
  page,
  totalPages,
  hasPrevious,
  hasNext,
  onPageChange,
}: Props) {
  return (
    <div className="mt-6 flex items-center justify-end gap-2">
      <button
        onClick={() => onPageChange(page - 1)}
        disabled={!hasPrevious}
        className="rounded-lg border px-4 py-2 text-sm disabled:cursor-not-allowed disabled:opacity-50"
      >
        Previous
      </button>

      <span className="text-sm text-slate-600">
        Page {page} / {totalPages}
      </span>

      <button
        onClick={() => onPageChange(page + 1)}
        disabled={!hasNext}
        className="rounded-lg border px-4 py-2 text-sm disabled:cursor-not-allowed disabled:opacity-50"
      >
        Next
      </button>
    </div>
  );
}

export default NotePagination;