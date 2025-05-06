"use client";

import { useState } from "react";
import {
  Collapsible,
  CollapsibleContent,
  CollapsibleTrigger,
} from "@/components/ui/collapsible";
import { Button } from "@/components/ui/button";

type Request = {
  id: number;
  user: string;
  status: string;
  details: string;
};

const requests: Request[] = [
  {
    id: 1,
    user: "Alice",
    status: "Pending",
    details: "Requesting access to project X.",
  },
  {
    id: 2,
    user: "Bob",
    status: "Approved",
    details: "Requested more storage last week.",
  },
];

export default function RequestTable() {
  const [openRow, setOpenRow] = useState<number | null>(null);

  return (
    <div className="flex-1 overflow-x-auto p-4">
      <table className="min-w-full border border-gray-200 text-left">
        <thead>
          <tr className="bg-gray-100">
            <th className="p-2">User</th>
            <th className="p-2">Status</th>
            <th className="p-2">Action</th>
          </tr>
        </thead>
        <tbody>
          {requests.map((req) => (
            <Collapsible
              key={req.id}
              open={openRow === req.id}
              onOpenChange={() =>
                setOpenRow(openRow === req.id ? null : req.id)
              }
              asChild>
              <>
                <tr className="border-t">
                  <td className="p-2">{req.user}</td>
                  <td className="p-2">{req.status}</td>
                  <td className="p-2">
                    <CollapsibleTrigger asChild>
                      <Button variant="outline" size="sm">
                        {openRow === req.id ? "Hide" : "Details"}
                      </Button>
                    </CollapsibleTrigger>
                  </td>
                </tr>
                <CollapsibleContent asChild>
                  <tr className="bg-gray-50">
                    <td colSpan={3} className="p-3 text-sm text-gray-700">
                      {req.details}
                    </td>
                  </tr>
                </CollapsibleContent>
              </>
            </Collapsible>
          ))}
        </tbody>
      </table>
    </div>
  );
}
