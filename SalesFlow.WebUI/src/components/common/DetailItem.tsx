type Props = {
  label: string;
  value?: React.ReactNode;
};

function DetailItem({
  label,
  value,
}: Props) {
  return (
    <div>

      <p className="text-sm text-slate-500">
        {label}
      </p>

      <h4 className="mt-1 font-semibold text-slate-900">
        {value || "-"}
      </h4>

    </div>
  );
}

export default DetailItem;