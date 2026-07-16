import { useNavigate } from "react-router-dom";

import { toast } from "sonner";

import PageHeader from "@/components/common/PageHeader";

import NoteForm from "../components/NoteForm";

import { createNote } from "../services/noteService";

import type {
  NoteFormData,
} from "../schemas/noteSchema";

function NoteCreatePage() {

  const navigate =
    useNavigate();

  async function handleCreate(
    data: NoteFormData
  ) {

    const response =
      await createNote(data);

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

  return (

    <div className="space-y-8">

      <PageHeader
        title="Create Note"
        description="Create a new note."
      />

      <NoteForm
        submitText="Create Note"
        onSubmit={handleCreate}
      />

    </div>

  );
}

export default NoteCreatePage;