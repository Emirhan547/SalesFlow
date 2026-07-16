import { useMemo, useState } from "react";

import { toast } from "sonner";

import EmptyState from "@/components/common/EmptyState";
import LoadingState from "@/components/common/LoadingState";

import AttachmentHeader from "../components/AttachmentHeader";
import AttachmentPagination from "../components/AttachmentPagination";
import AttachmentSearch from "../components/AttachmentSearch";
import AttachmentTable from "../components/AttachmentTable";
import AttachmentUpload from "../components/AttachmentUpload";

import { useAttachments } from "../hooks/useAttachments";
import { useDebounce } from "../hooks/useDebounce";

function AttachmentListPage() {

  const [search, setSearch] =
    useState("");

  const [page, setPage] =
    useState(1);

  const [showUpload, setShowUpload] =
    useState(false);

  /*
    Şimdilik sabit.
    Daha sonra Customer Detail sayfasından
    customerId route üzerinden gelecek.
  */
  const customerId = 1;

  const debouncedSearch =
    useDebounce(search);

  const filter = useMemo(
    () => ({
      page,
      pageSize: 10,
      search: debouncedSearch,
    }),
    [page, debouncedSearch]
  );

  const {
    data,
    loading,
    error,
    reload,
  } = useAttachments(filter);

  if (loading)
    return <LoadingState />;

  if (error)
    return (
      <div className="rounded-2xl border border-red-200 bg-red-50 p-6 text-red-600">
        {error}
      </div>
    );

  if (!data)
    return (
      <EmptyState
        title="No Attachments"
        description="There are no attachments."
      />
    );

  return (
    <div className="space-y-8">

      <AttachmentHeader
        onUpload={() =>
          setShowUpload(!showUpload)
        }
      />

      {showUpload && (

        <AttachmentUpload
          customerId={customerId}
          onUploaded={() => {

            toast.success(
              "Attachment uploaded."
            );

            reload();

            setShowUpload(false);

          }}
        />

      )}

      <AttachmentSearch
        value={search}
        onChange={(value) => {

          setSearch(value);

          setPage(1);

        }}
      />

      <AttachmentTable
        attachments={data.items}
        onDeleted={reload}
      />

      <AttachmentPagination
        page={data.page}
        totalPages={data.totalPages}
        hasPrevious={data.hasPrevious}
        hasNext={data.hasNext}
        onPageChange={setPage}
      />

    </div>
  );
}

export default AttachmentListPage;