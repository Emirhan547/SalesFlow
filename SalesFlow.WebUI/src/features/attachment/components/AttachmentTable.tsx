import { useState } from "react";

import {
  Download,
  Trash2,
} from "lucide-react";

import { toast } from "sonner";

import DeleteDialog from "@/components/common/DeleteDialog";

import {
  deleteAttachment,
  downloadAttachment,
} from "../services/attachmentService";

import type { Attachment } from "../types/Attachment";

type Props = {
  attachments: Attachment[];
  onDeleted: () => void;
};

function AttachmentTable({
  attachments,
  onDeleted,
}: Props) {

  const [selectedId, setSelectedId] =
    useState<number>();

  async function handleDelete() {

    if (!selectedId)
      return;

    const response =
      await deleteAttachment(selectedId);

    if (!response.isSuccess) {

      toast.error(response.message);

      return;

    }

    toast.success(response.message);

    setSelectedId(undefined);

    onDeleted();

  }

  async function handleDownload(
    attachment: Attachment
  ) {

    const blob =
      await downloadAttachment(
        attachment.id
      );

    const url =
      window.URL.createObjectURL(blob);

    const link =
      document.createElement("a");

    link.href = url;

    link.download =
      attachment.fileName;

    link.click();

    window.URL.revokeObjectURL(url);

  }

  return (
    <>
      <div className="overflow-hidden rounded-2xl border bg-white">

        <table className="w-full">

          <thead className="bg-slate-50">

            <tr>

              <th className="px-6 py-4 text-left">
                File
              </th>

              <th className="px-6 py-4 text-left">
                Type
              </th>

              <th className="px-6 py-4 text-left">
                Size
              </th>

              <th className="px-6 py-4 text-right">
                Actions
              </th>

            </tr>

          </thead>

          <tbody>

            {attachments.map((attachment) => (

              <tr
                key={attachment.id}
                className="border-t"
              >

                <td className="px-6 py-4">
                  {attachment.fileName}
                </td>

                <td className="px-6 py-4">
                  {attachment.contentType}
                </td>

                <td className="px-6 py-4">
                  {(attachment.fileSize / 1024).toFixed(1)} KB
                </td>

                <td className="px-6 py-4">

                  <div className="flex justify-end gap-2">

                    <button
                      onClick={() =>
                        handleDownload(
                          attachment
                        )
                      }
                    >

                      <Download
                        size={18}
                      />

                    </button>

                    <button
                      onClick={() =>
                        setSelectedId(
                          attachment.id
                        )
                      }
                    >

                      <Trash2
                        size={18}
                      />

                    </button>

                  </div>

                </td>

              </tr>

            ))}

          </tbody>

        </table>

      </div>

      <DeleteDialog
        open={
          selectedId !== undefined
        }
        onOpenChange={() =>
          setSelectedId(undefined)
        }
        title="Delete Attachment"
        description="Are you sure you want to delete this attachment?"
        onConfirm={handleDelete}
      />

    </>
  );
}

export default AttachmentTable;