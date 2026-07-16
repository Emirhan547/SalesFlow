import { useParams } from "react-router-dom";

import LoadingState from "@/components/common/LoadingState";
import PageHeader from "@/components/common/PageHeader";

import { useTag } from "../hooks/useTag";

function TagDetailPage() {

  const { id } =
    useParams();

  const {
    tag,
    loading,
    error,
  } = useTag(Number(id));

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
        title="Tag Detail"
        description="Tag information"
      />

      <div className="rounded-2xl border bg-white p-8 space-y-6">

        <div>

          <h3 className="text-sm text-slate-500">
            Name
          </h3>

          <p className="mt-1">
            {tag.name}
          </p>

        </div>

        <div>

          <h3 className="text-sm text-slate-500">
            Color
          </h3>

          <div className="mt-2 flex items-center gap-3">

            {tag.color && (

              <span
                className="h-6 w-6 rounded-full border"
                style={{
                  backgroundColor: tag.color,
                }}
              />

            )}

            <span>
              {tag.color ?? "-"}
            </span>

          </div>

        </div>

      </div>

    </div>

  );
}

export default TagDetailPage;