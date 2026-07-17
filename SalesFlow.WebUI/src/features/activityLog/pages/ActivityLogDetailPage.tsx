import { useParams } from "react-router-dom";

import DetailItem from "@/components/common/DetailItem";
import LoadingState from "@/components/common/LoadingState";
import PageHeader from "@/components/common/PageHeader";
import Card from "@/components/ui/Card";

import { useActivityLog } from "../hooks/useActivityLog";

import { ActivityActions } from "../types/ActivityAction";

function ActivityLogDetailPage() {

  const { id } =
    useParams();

  const {
    activityLog,
    loading,
    error,
  } = useActivityLog(Number(id));

  function getActionName(
    action: number
  ) {

    switch (action) {

      case ActivityActions.Create:
        return "Create";

      case ActivityActions.Update:
        return "Update";

      case ActivityActions.Delete:
        return "Delete";

      case ActivityActions.Convert:
        return "Convert";

      case ActivityActions.Login:
        return "Login";

      case ActivityActions.Logout:
        return "Logout";

      default:
        return "-";

    }

  }

  if (loading)
    return <LoadingState />;

  if (error)
    return (
      <div className="rounded-xl border border-red-200 bg-red-50 p-6 text-red-600">
        {error}
      </div>
    );

  if (!activityLog)
    return (
      <div className="rounded-xl bg-white p-8 text-center shadow">
        Activity log not found.
      </div>
    );

  return (

    <div className="space-y-8">

      <PageHeader
        title="Activity Log Detail"
        description="Activity information"
      />

      <Card title="General Information">

        <div className="grid gap-6 md:grid-cols-2">

          <DetailItem
            label="Action"
            value={getActionName(activityLog.action)}
          />

          <DetailItem
            label="Entity"
            value={activityLog.entityName}
          />

          <DetailItem
            label="Entity Id"
            value={activityLog.entityId}
          />

          <DetailItem
            label="User"
            value={activityLog.userName}
          />

          <DetailItem
            label="Created Date"
            value={
              new Date(
                activityLog.createdDate
              ).toLocaleString()
            }
          />

        </div>

      </Card>

      <Card title="Description">

        <p>
          {activityLog.description}
        </p>

      </Card>

    </div>

  );
}

export default ActivityLogDetailPage;