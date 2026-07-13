import Skeleton from "@/components/ui/Skeleton";

function DashboardSkeleton() {
  return (
    <div className="space-y-8">

      <Skeleton className="h-12 w-72" />

      <div className="grid gap-6 md:grid-cols-2 xl:grid-cols-5">

        {Array.from({ length: 5 }).map((_, index) => (

          <div
            key={index}
            className="rounded-2xl bg-white p-6 shadow"
          >
            <Skeleton className="h-4 w-24" />

            <Skeleton className="mt-6 h-10 w-28" />

            <Skeleton className="mt-8 h-16 w-16 rounded-2xl" />

          </div>

        ))}

      </div>

      <div className="grid gap-8 xl:grid-cols-3">

        <Skeleton className="h-96 xl:col-span-2" />

        <Skeleton className="h-96" />

      </div>

    </div>
  );
}

export default DashboardSkeleton;