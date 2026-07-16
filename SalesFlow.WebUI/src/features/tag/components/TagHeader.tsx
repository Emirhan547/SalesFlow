import { Plus } from "lucide-react";

import PageHeader from "@/components/common/PageHeader";

import { Button } from "@/components/ui/button";

type Props = {
  onCreate: () => void;
};

function TagHeader({
  onCreate,
}: Props) {
  return (

    <PageHeader
      title="Tags"
      description="Manage customer tags."
      action={
        <Button onClick={onCreate}>

          <Plus
            size={18}
            className="mr-2"
          />

          New Tag

        </Button>
      }
    />

  );
}

export default TagHeader;