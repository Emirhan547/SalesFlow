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
     <div className="overflow-hidden rounded-xl border border-slate-200 bg-white shadow-sm">

        <table className="w-full">

          <thead className="bg-slate-50">

           <tr className="text-left text-xs font-semibold uppercase tracking-wide text-slate-600 border-b border-slate-200">

             <th className="px-6 py-4">
                Content
              </th>

             <th className="px-6 py-4">
 Customer
</th>

<th className="px-6 py-4">
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
               className="border-b border-slate-100 transition-all duration-200 hover:bg-blue-50/50"
              >

               <td className="px-6 py-5 max-w-md truncate text-sm text-slate-700">
                  {note.content}
                </td>

               <td className="px-6 py-5 text-sm text-slate-700">
  {note.customerName}
</td>

               <td className="px-6 py-5 text-sm text-slate-700">
  {note.createdByName ?? "-"}
</td>

                <td className="px-6 py-5">

                  <div className="flex justify-end gap-1">

                    <button
                      onClick={() =>
                        navigate(`/notes/${note.id}`)
                      }
                      className="rounded-lg p-2 text-slate-500 transition-all hover:bg-blue-100 hover:text-blue-600"
                      title="View"
                    >
                      <Eye size={16} />
                    </button>

                    <button
                      onClick={() =>
                        navigate(`/notes/edit/${note.id}`)
                      }
                      className="rounded-lg p-2 text-slate-500 transition-all hover:bg-amber-100 hover:text-amber-600"
                      title="Edit"
                    >
                      <Pencil size={16} />
                    </button>

                    <button
                      onClick={() =>
                        setSelectedId(note.id)
                      }
                      className="rounded-lg p-2 text-slate-500 transition-all hover:bg-red-100 hover:text-red-600"
                      title="Delete"
                    >
                      <Trash2 size={16} />
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