import { useNavigate } from "react-router-dom";

import { toast } from "sonner";

import PageHeader from "@/components/common/PageHeader";

import TagForm from "../components/TagForm";

import { createTag } from "../services/tagService";

import type {
  TagFormData,
} from "../schemas/tagSchema";

function TagCreatePage() {

  const navigate =
    useNavigate();

  async function handleCreate(
    data: TagFormData
  ) {

    const response =
      await createTag(data);

    if (!response.isSuccess) {

      toast.error(response.message);

      return;

    }

    toast.success(response.message);

    navigate("/tags");

  }

  return (

    <div className="space-y-8">

      <PageHeader
        title="Create Tag"
        description="Create a new tag."
      />

      <TagForm
        submitText="Create Tag"
        onSubmit={handleCreate}
      />

    </div>

  );
}

export default TagCreatePage;