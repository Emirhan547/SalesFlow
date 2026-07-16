import { CalendarPlus } from "lucide-react";

import { Button } from "@/components/ui/button";

type Props = {
  onCreate: () => void;
};

function MeetingHeader({
  onCreate,
}: Props) {
  return (
    <Button
      onClick={onCreate}
    >
      <CalendarPlus
        className="mr-2"
        size={18}
      />

      New Meeting

    </Button>
  );
}

export default MeetingHeader;