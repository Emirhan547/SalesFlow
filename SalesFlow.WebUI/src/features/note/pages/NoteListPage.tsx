import { useMemo, useState } from "react";

import { useNavigate } from "react-router-dom";

import LoadingState from "@/components/common/LoadingState";

import NoteHeader from "../components/NoteHeader";
import NotePagination from "../components/NotePagination";
import NoteSearch from "../components/NoteSearch";
import NoteTable from "../components/NoteTable";

import { useNotes } from "../hooks/useNotes";

function NoteListPage() {

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
  } = useNotes(filter);

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

      <NoteHeader
        onCreate={() =>
          navigate("/notes/create")
        }
      />

      <NoteSearch
        value={search}
        onChange={(value) => {

          setPage(1);

          setSearch(value);

        }}
      />

      <NoteTable
        notes={data?.items ?? []}
        onDeleted={reload}
      />

      {data && (

        <NotePagination
          page={data.page}
          totalPages={data.totalPages}
          hasNext={data.hasNext}
          hasPrevious={data.hasPrevious}
          onPageChange={setPage}
        />

      )}

    </div>
  );
}

export default NoteListPage;