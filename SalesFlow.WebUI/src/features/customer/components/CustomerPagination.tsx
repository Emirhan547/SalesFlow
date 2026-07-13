type Props = {
  page: number;
  totalPages: number;
  onChange: (page: number) => void;
};

function CustomerPagination({
  page,
  totalPages,
  onChange,
}: Props) {
  return (
    <div className="flex justify-end gap-3 mt-6">

      <button
        disabled={page === 1}
        onClick={() => onChange(page - 1)}
        className="border rounded px-4 py-2"
      >
        Previous
      </button>

      <span className="flex items-center">
        {page} / {totalPages}
      </span>

      <button
        disabled={page === totalPages}
        onClick={() => onChange(page + 1)}
        className="border rounded px-4 py-2"
      >
        Next
      </button>

    </div>
  );
}

export default CustomerPagination;