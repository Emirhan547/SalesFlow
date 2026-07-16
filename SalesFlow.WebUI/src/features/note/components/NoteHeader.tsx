import { Plus } from "lucide-react";

import PageHeader from "@/components/common/PageHeader";

import { Button } from "@/components/ui/button";

type Props = {
  onCreate: () => void;
};

function NoteHeader({
  onCreate,
}: Props) {
  return (
    <PageHeader
      title="Notes"
      description="Manage customer notes."
      action={
        <Button
          onClick={onCreate}
        >

          <Plus
            size={18}
            className="mr-2"
          />

          New Note

        </Button>
      }
    />
  );
}

export default NoteHeader;