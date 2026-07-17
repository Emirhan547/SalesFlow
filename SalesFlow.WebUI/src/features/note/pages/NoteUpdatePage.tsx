import {
  useNavigate,
  useParams,
} from "react-router-dom";

import { toast } from "sonner";

import LoadingState from "@/components/common/LoadingState";
import PageHeader from "@/components/common/PageHeader";

import NoteForm from "../components/NoteForm";

import { useNote } from "../hooks/useNote";

import { updateNote } from "../services/noteService";

import type {
  NoteFormData,
} from "../schemas/noteSchema";

function NoteUpdatePage() {

  const { id } =
    useParams();

  const navigate =
    useNavigate();

  const {

    note,

    loading,

    error,

  } = useNote(
    Number(id)
  );

  async function handleUpdate(
    data: NoteFormData
  ) {

    if (!note)
      return;

    const response =
      await updateNote({

        id: note.id,

        ...data,

      });

    if (!response.isSuccess) {

      toast.error(
        response.message
      );

      return;

    }

    toast.success(
      response.message
    );

    navigate("/notes");

  }

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
        title="Update Note"
        description="Update note."
      />

      <NoteForm
        submitText="Update Note"
        defaultValues={{

          content:
            note.content,

          customerId:
            note.customerId,

         

        }}
        onSubmit={handleUpdate}
      />

    </div>

  );
}

export default NoteUpdatePage;