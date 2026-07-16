type Props = {
  page: number;
  totalPages: number;
  hasPrevious: boolean;
  hasNext: boolean;
  onPageChange: (
    page: number
  ) => void;
};

function MeetingPagination({
  page,
  totalPages,
  hasPrevious,
  hasNext,
  onPageChange,
}: Props) {
  return (
    <div className="flex items-center justify-between">

      <button
        disabled={!hasPrevious}
        onClick={() =>
          onPageChange(page - 1)
        }
        className="rounded-xl border border-slate-200 px-5 py-2 disabled:opacity-50"
      >
        Previous
      </button>

      <span className="font-medium">

        Page {page} / {totalPages}

      </span>

      <button
        disabled={!hasNext}
        onClick={() =>
          onPageChange(page + 1)
        }
        className="rounded-xl border border-slate-200 px-5 py-2 disabled:opacity-50"
      >
        Next
      </button>

    </div>
  );
}

export default MeetingPagination;