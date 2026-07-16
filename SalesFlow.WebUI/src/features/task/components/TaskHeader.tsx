import { Plus } from "lucide-react";
import { useNavigate } from "react-router-dom";

import PageHeader from "@/components/common/PageHeader";
import { Button } from "@/components/ui/button";

function TaskHeader() {

  const navigate =
    useNavigate();

  return (
    <PageHeader
      title="Tasks"
      description="Manage all tasks."
      action={
        <Button
          onClick={() =>
            navigate("/tasks/create")
          }
        >
          <Plus
            size={18}
            className="mr-2"
          />

          New Task

        </Button>
      }
    />
  );
}

export default TaskHeader;