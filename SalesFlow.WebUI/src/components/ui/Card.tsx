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
      className={`overflow-hidden rounded-3xl border border-slate-200 bg-white shadow-sm transition-shadow duration-300 hover:shadow-lg ${className}`}
    >

      {(title || action) && (

        <div className="flex items-center justify-between border-b border-slate-100 px-7 py-6">

          <div>

            {title && (
              <h2 className="text-xl font-bold text-slate-900">
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

      <div className="p-7">

        {children}

      </div>

    </div>
  );
}

export default Card;