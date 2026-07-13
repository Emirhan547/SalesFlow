type Props = {
  search: string;
  onSearchChange: (value: string) => void;
  onCreate: () => void;
};

function CustomerToolbar({
  search,
  onSearchChange,
  onCreate,
}: Props) {
  return (
    <div className="flex items-center justify-between mb-6">

      <input
        className="border rounded-lg px-4 py-2 w-80"
        placeholder="Search customer..."
        value={search}
        onChange={(e) => onSearchChange(e.target.value)}
      />

      <button
        onClick={onCreate}
        className="bg-black text-white px-5 py-2 rounded-lg"
      >
        New Customer
      </button>

    </div>
  );
}

export default CustomerToolbar;