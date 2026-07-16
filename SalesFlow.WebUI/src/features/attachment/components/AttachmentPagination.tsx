import Pagination from "@/components/common/Pagination";

type Props = {
  page: number;
  totalPages: number;
  hasPrevious: boolean;
  hasNext: boolean;
  onPageChange: (page: number) => void;
};

function AttachmentPagination(props: Props) {
  return (
    <Pagination
      {...props}
    />
  );
}

export default AttachmentPagination;