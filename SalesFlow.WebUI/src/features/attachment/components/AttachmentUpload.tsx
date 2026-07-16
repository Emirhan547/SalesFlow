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

  const [inputKey, setInputKey] =
    useState(0);

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

    setInputKey((x) => x + 1);

    onUploaded();

  }

  return (
    <div className="flex items-center gap-3">

      <Input
        key={inputKey}
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