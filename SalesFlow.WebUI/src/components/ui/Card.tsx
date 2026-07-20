type Props = {
  title?: string;
  subtitle?: string;
  action?: React.ReactNode;
  children: React.ReactNode;
  className?: string;
};

function Card({
  title,
  subtitle,
  action,
  children,
  className = "",
}: Props) {
  return (
    <div
      className={`overflow-hidden rounded-xl border border-slate-200 bg-white shadow-sm transition-all duration-200 hover:shadow-md hover:border-slate-300 ${className}`}
    >

      {(title || action) && (

        <div className="flex items-center justify-between border-b border-slate-100 px-6 py-5">

          <div>

            {title && (
              <h2 className="text-lg font-semibold text-slate-900">
                {title}
              </h2>
            )}

            {subtitle && (
              <p className="mt-1 text-sm text-slate-500">
                {subtitle}
              </p>
            )}

          </div>

          {action}

        </div>

      )}

      <div className="p-6">

        {children}

      </div>

    </div>
  );
}

export default Card;