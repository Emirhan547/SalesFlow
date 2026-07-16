import { useMemo, useState } from "react";

import { useNavigate } from "react-router-dom";

import LoadingState from "@/components/common/LoadingState";

import TagHeader from "../components/TagHeader";
import TagPagination from "../components/TagPagination";
import TagSearch from "../components/TagSearch";
import TagTable from "../components/TagTable";

import { useTags } from "../hooks/useTags";

function TagListPage() {

  const navigate = useNavigate();

  const [page, setPage] =
    useState(1);

  const [search, setSearch] =
    useState("");

  const filter = useMemo(() => ({
    page,
    pageSize: 10,
    search,
  }), [page, search]);

  const {
    data,
    loading,
    error,
    reload,
  } = useTags(filter);

  if (loading)
    return <LoadingState />;

  if (error)
    return (
      <div className="rounded-xl border border-red-200 bg-red-50 p-6 text-red-600">
        {error}
      </div>
    );

  return (

    <div className="space-y-6">

      <TagHeader
        onCreate={() =>
          navigate("/tags/create")
        }
      />

      <TagSearch
        value={search}
        onChange={(value) => {

          setPage(1);

          setSearch(value);

        }}
      />

      <TagTable
        tags={data?.items ?? []}
        onDeleted={reload}
      />

      {data && (

        <TagPagination
          page={data.page}
          totalPages={data.totalPages}
          hasPrevious={data.hasPrevious}
          hasNext={data.hasNext}
          onPageChange={setPage}
        />

      )}

    </div>

  );
}

export default TagListPage;