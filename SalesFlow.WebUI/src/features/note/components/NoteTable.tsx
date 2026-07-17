import { useState } from "react";
import { useNavigate } from "react-router-dom";

import {
  Eye,
  Pencil,
  Trash2,
} from "lucide-react";

import { toast } from "sonner";


import { deleteNote } from "../services/noteService";

import type { Note } from "../types/Note";
import DeleteDialog from "@/components/common/DeleteDialog";

type Props = {
  notes: Note[];
  onDeleted: () => void;
};

function NoteTable({
  notes,
  onDeleted,
}: Props) {

  const navigate =
    useNavigate();

  const [selectedId, setSelectedId] =
    useState<number>();

  async function handleDelete() {

    if (!selectedId)
      return;

    const response =
      await deleteNote(selectedId);

    if (!response.isSuccess) {

      toast.error(response.message);

      return;

    }

    toast.success(response.message);

    setSelectedId(undefined);

    onDeleted();

  }

  return (
    <>
      <div className="overflow-hidden rounded-2xl border bg-white">

        <table className="w-full">

          <thead className="bg-slate-50">

            <tr>

              <th className="px-6 py-4 text-left">
                Content
              </th>

             <th className="px-6 py-4 text-left">
  Customer
</th>

<th className="px-6 py-4 text-left">
  Created By
</th>

              <th className="px-6 py-4 text-right">
                Actions
              </th>

            </tr>

          </thead>

          <tbody>

            {notes.map((note) => (

              <tr
                key={note.id}
                className="border-t"
              >

                <td className="px-6 py-4 max-w-md truncate">
                  {note.content}
                </td>

                <td className="px-6 py-4">
  {note.customerName}
</td>

               <td className="px-6 py-4">
  {note.createdByName ?? "-"}
</td>

                <td className="px-6 py-4">

                  <div className="flex justify-end gap-2">

                    <button
                      onClick={() =>
                        navigate(`/notes/${note.id}`)
                      }
                    >
                      <Eye size={18} />
                    </button>

                    <button
                      onClick={() =>
                        navigate(`/notes/edit/${note.id}`)
                      }
                    >
                      <Pencil size={18} />
                    </button>

                    <button
                      onClick={() =>
                        setSelectedId(note.id)
                      }
                    >
                      <Trash2 size={18} />
                    </button>

                  </div>

                </td>

              </tr>

            ))}

          </tbody>

        </table>

      </div>

      <DeleteDialog
        open={selectedId !== undefined}
        onOpenChange={() =>
          setSelectedId(undefined)
        }
        title="Delete Note"
        description="Are you sure you want to delete this note?"
        onConfirm={handleDelete}
      />
    </>
  );
}

export default NoteTable;