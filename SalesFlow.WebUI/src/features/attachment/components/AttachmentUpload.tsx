import { useState } from "react";

import { toast } from "sonner";

import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";

import { createAttachment } from "../services/attachmentService";

type Props = {
  customerId: number;
  onUploaded: () => void;
};

function AttachmentUpload({
  customerId,
  onUploaded,
}: Props) {

  const [file, setFile] =
    useState<File>();

  async function handleUpload() {

    if (!file) {

      toast.error(
        "Please select a file."
      );

      return;

    }

    const response =
      await createAttachment({

        file,

        customerId,

      });

    if (!response.isSuccess) {

      toast.error(response.message);

      return;

    }

    toast.success(response.message);

    setFile(undefined);

    onUploaded();

  }

  return (
    <div className="flex gap-3">

      <Input
        type="file"
        onChange={(e) =>
          setFile(
            e.target.files?.[0]
          )
        }
      />

      <Button
        onClick={handleUpload}
      >
        Upload
      </Button>

    </div>
  );
}

export default AttachmentUpload;