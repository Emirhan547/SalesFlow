type Props = {
  page: number;
  totalPages: number;
  hasPrevious: boolean;
  hasNext: boolean;
  onPageChange: (page: number) => void;
};

function CustomerPagination({
  page,
  totalPages,
  hasPrevious,
  hasNext,
  onPageChange,
}: Props) {
  return (
    <div className="flex items-center justify-between rounded-2xl bg-white p-5 shadow">

      <button
        disabled={!hasPrevious}
        onClick={() => onPageChange(page - 1)}
        className="rounded-xl border px-4 py-2 disabled:opacity-40"
      >
        Previous
      </button>

      <span className="font-medium">
        Page {page} / {totalPages}
      </span>

      <button
        disabled={!hasNext}
        onClick={() => onPageChange(page + 1)}
        className="rounded-xl border px-4 py-2 disabled:opacity-40"
      >
        Next
      </button>

    </div>
  );
}

export default CustomerPagination;