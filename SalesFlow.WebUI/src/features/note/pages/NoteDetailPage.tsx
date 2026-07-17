import { useParams } from "react-router-dom";

import LoadingState from "@/components/common/LoadingState";
import PageHeader from "@/components/common/PageHeader";

import { useNote } from "../hooks/useNote";

function NoteDetailPage() {

  const { id } = useParams();

  const {
    note,
    loading,
    error,
  } = useNote(Number(id));

  if (loading)
    return <LoadingState />;

  if (error)
    return (
      <div className="rounded-xl border border-red-200 bg-red-50 p-6 text-red-600">
        {error}
      </div>
    );

  if (!note)
    return null;

  return (

    <div className="space-y-8">

      <PageHeader
        title="Note Detail"
        description="Note information"
      />

      <div className="rounded-2xl border bg-white p-8 space-y-6">

        <div>

          <h3 className="text-sm text-slate-500">
            Content
          </h3>

          <p className="mt-1 whitespace-pre-wrap">
            {note.content}
          </p>

        </div>

        <div className="grid grid-cols-2 gap-6">

          <div>

            <h3 className="text-sm text-slate-500">
             Customer
            </h3>

            <p className="mt-1">
            {note.customerName}
            </p>

          </div>

          <div>

            <h3 className="text-sm text-slate-500">
              Created By
            </h3>

            <p className="mt-1">
            {note.createdByName ?? "-"}
            </p>

          </div>

        </div>

      </div>

    </div>

  );
}

export default NoteDetailPage;