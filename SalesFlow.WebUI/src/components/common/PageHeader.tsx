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
    <div className="flex flex-col justify-between gap-8 md:flex-row md:items-end">

      <div>

        <h1 className="text-3xl font-bold tracking-tight text-slate-900">
          {title}
        </h1>

        {description && (
          <p className="mt-3 text-slate-500">
            {description}
          </p>
        )}

      </div>

      {action}

    </div>
  );
}

export default PageHeader;