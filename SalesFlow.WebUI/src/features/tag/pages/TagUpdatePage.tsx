import {
  useNavigate,
  useParams,
} from "react-router-dom";

import { toast } from "sonner";

import LoadingState from "@/components/common/LoadingState";
import PageHeader from "@/components/common/PageHeader";

import TagForm from "../components/TagForm";

import { useTag } from "../hooks/useTag";

import { updateTag } from "../services/tagService";

import type {
  TagFormData,
} from "../schemas/tagSchema";

function TagUpdatePage() {

  const { id } =
    useParams();

  const navigate =
    useNavigate();

  const {

    tag,

    loading,

    error,

  } = useTag(Number(id));

  async function handleUpdate(
    data: TagFormData
  ) {

    if (!tag)
      return;

    const response =
      await updateTag({

        id: tag.id,

        ...data,

      });

    if (!response.isSuccess) {

      toast.error(response.message);

      return;

    }

    toast.success(response.message);

    navigate("/tags");

  }

  if (loading)
    return <LoadingState />;

  if (error)
    return (
      <div className="rounded-xl border border-red-200 bg-red-50 p-6 text-red-600">
        {error}
      </div>
    );

  if (!tag)
    return null;

  return (

    <div className="space-y-8">

      <PageHeader
        title="Update Tag"
        description="Update tag."
      />

      <TagForm
        submitText="Update Tag"
        defaultValues={{
          name: tag.name,
          color: tag.color ?? "",
        }}
        onSubmit={handleUpdate}
      />

    </div>

  );
}

export default TagUpdatePage;