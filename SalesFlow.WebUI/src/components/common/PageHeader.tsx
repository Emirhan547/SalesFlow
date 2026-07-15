type Props = {
  title: string;
  description?: string;
  action?: React.ReactNode;
};

function PageHeader({
  title,
  description,
  action,
}: Props) {
  return (
    <div className="flex flex-col justify-between gap-6 md:flex-row md:items-center">

      <div>

        <h1 className="text-4xl font-bold tracking-tight text-slate-900">
          {title}
        </h1>

        {description && (
          <p className="mt-2 text-slate-500">
            {description}
          </p>
        )}

      </div>

      {action}

    </div>
  );
}

export default PageHeader;