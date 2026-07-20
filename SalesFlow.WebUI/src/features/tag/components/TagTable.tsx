import { useState } from "react";
import { useNavigate } from "react-router-dom";
import {
  Eye,
  Pencil,
  Trash2,
} from "lucide-react";

import { toast } from "sonner";


import { deleteTag } from "../services/tagService";

import type { Tag } from "../types/Tag";
import DeleteDialog from "@/components/common/DeleteDialog";

type Props = {
  tags: Tag[];
  onDeleted: () => void;
};

function TagTable({
  tags,
  onDeleted,
}: Props) {

  const navigate = useNavigate();

  const [selectedId, setSelectedId] =
    useState<number>();

  async function handleDelete() {

    if (!selectedId)
      return;

    const response =
      await deleteTag(selectedId);

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
                Name
              </th>

              <th className="px-6 py-4">
                Color
              </th>

              <th className="px-6 py-4 text-right">
                Actions
              </th>

            </tr>

          </thead>

          <tbody>

            {tags.map((tag) => (

              <tr
                key={tag.id}
                className="border-b border-slate-100 transition-all duration-200 hover:bg-blue-50/50"
              >

                <td className="px-6 py-5 text-sm font-semibold text-slate-900">
                  {tag.name}
                </td>

                <td className="px-6 py-5">

                  {tag.color
                    ? (
                      <div className="flex items-center gap-2">

                        <span
                          className="h-4 w-4 rounded-full border border-slate-300"
                          style={{
                            backgroundColor: tag.color,
                          }}
                        />

                        <span className="text-sm text-slate-600">{tag.color}</span>

                      </div>
                    )
                    : <span className="text-sm text-slate-500">-</span>}

                </td>

                <td className="px-6 py-5">

                  <div className="flex justify-end gap-1">

                    <button
                      onClick={() =>
                        navigate(`/tags/${tag.id}`)
                      }
                      className="rounded-lg p-2 text-slate-500 transition-all hover:bg-blue-100 hover:text-blue-600"
                      title="View"
                    >
                      <Eye size={16} />
                    </button>

                    <button
                      onClick={() =>
                        navigate(`/tags/edit/${tag.id}`)
                      }
                      className="rounded-lg p-2 text-slate-500 transition-all hover:bg-amber-100 hover:text-amber-600"
                      title="Edit"
                    >
                      <Pencil size={16} />
                    </button>

                    <button
                      onClick={() =>
                        setSelectedId(tag.id)
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
        title="Delete Tag"
        description="Are you sure you want to delete this tag?"
        onConfirm={handleDelete}
      />

    </>
  );
}

export default TagTable;