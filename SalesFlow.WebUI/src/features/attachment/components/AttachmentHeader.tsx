import {
  Download,
  Upload,
} from "lucide-react";

import { Button } from "@/components/ui/button";

import PageHeader from "@/components/common/PageHeader";

type Props = {
  onUpload: () => void;
};

function AttachmentHeader({
  onUpload,
}: Props) {
  return (
    <PageHeader
      title="Attachments"
      description="Manage customer attachments."
      action={
        <Button onClick={onUpload}>

          <Upload
            size={18}
            className="mr-2"
          />

          Upload File

        </Button>
      }
    />
  );
}

export default AttachmentHeader;