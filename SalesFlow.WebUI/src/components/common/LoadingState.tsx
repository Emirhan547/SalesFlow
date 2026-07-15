import Skeleton from "@/components/ui/Skeleton";

function LoadingState() {
  return (
    <div className="space-y-6">

      <Skeleton className="h-10 w-64" />

      <Skeleton className="h-96 w-full rounded-3xl" />

    </div>
  );
}

export default LoadingState;