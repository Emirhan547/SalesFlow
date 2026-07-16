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
      <div className="overflow-hidden rounded-2xl border bg-white">

        <table className="w-full">

          <thead className="bg-slate-50">

            <tr>

              <th className="px-6 py-4 text-left">
                Name
              </th>

              <th className="px-6 py-4 text-left">
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
                className="border-t"
              >

                <td className="px-6 py-4">
                  {tag.name}
                </td>

                <td className="px-6 py-4">

                  {tag.color
                    ? (
                      <div className="flex items-center gap-2">

                        <span
                          className="h-5 w-5 rounded-full border"
                          style={{
                            backgroundColor: tag.color,
                          }}
                        />

                        {tag.color}

                      </div>
                    )
                    : "-"}

                </td>

                <td className="px-6 py-4">

                  <div className="flex justify-end gap-2">

                    <button
                      onClick={() =>
                        navigate(`/tags/${tag.id}`)
                      }
                    >
                      <Eye size={18} />
                    </button>

                    <button
                      onClick={() =>
                        navigate(`/tags/edit/${tag.id}`)
                      }
                    >
                      <Pencil size={18} />
                    </button>

                    <button
                      onClick={() =>
                        setSelectedId(tag.id)
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
        title="Delete Tag"
        description="Are you sure you want to delete this tag?"
        onConfirm={handleDelete}
      />

    </>
  );
}

export default TagTable;