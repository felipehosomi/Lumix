ALTER PROCEDURE [dbo].[SP_SERIALBATCH_GET]
(
	@ItemCode		NVARCHAR(100),
	@BatchSerial	NVARCHAR(100),
	@BinCode		INT
)
AS
BEGIN
	declare @ItemManagement nvarchar(1), @i int, @_itemCode nvarchar(100), @_distNumber nvarchar(100), @_onHandQty int;

	create table #tmp
	(
		ItemCode nvarchar(100)
		, WhsCode nvarchar(100)
		, BinCode nvarchar(100)
		, OnHandQty int
		, DistNumber nvarchar(100)
	);

	create table #results
	(
		ItemCode nvarchar(100)
		, DistNumber nvarchar(100)
	);

	set @i = 0;

	select @ItemManagement =
		case when ManBtchNum = 'Y' then 'L'
		else case when ManSerNum = 'Y' then 'S'
		else '' end end from OITM where ItemCode = @ItemCode;

	if @ItemManagement = 'L' -- LOTE
	begin
		insert into #tmp
			select distinct
				T0.ItemCode
				, T0.WhsCode
				, T2.BinCode
				, 1
				, T1.DistNumber
			from OBBQ T0 with(nolock)
				inner join OBTN T1 with(nolock) on T0.SnBMDAbs = T1.AbsEntry
				inner join OBIN T2 with(nolock) on T0.BinAbs = T2.AbsEntry
			where 1=1
				and T0.OnHandQty <> 0
				and T0.ItemCode = isnull(@ItemCode, T0.ItemCode)
				and T1.DistNumber = isnull(@BatchSerial, T1.DistNumber)
				and T0.BinAbs = isnull(@BinCode, T0.BinAbs);
	end
	else if @ItemManagement = 'S' -- SERIE
	begin
		insert into #tmp
			select
				T0.ItemCode
				, T0.WhsCode
				, T2.BinCode
				, T0.OnHandQty
				, T1.DistNumber
			from OSBQ T0 with(nolock)
				inner join OSRN T1 with(nolock) on T0.SnBMDAbs = T1.AbsEntry
				inner join OBIN T2 with(nolock) on T0.BinAbs = T2.AbsEntry
			where 1=1
				and T0.OnHandQty <> 0
				and T0.ItemCode = isnull(@ItemCode, T0.ItemCode)
				and T1.DistNumber = isnull(@BatchSerial, T1.DistNumber)
				and T0.BinAbs = isnull(@BinCode, T0.BinAbs);
	end
	else
	begin
		insert into #tmp
			select distinct
				T0.ItemCode
				, T0.WhsCode
				, T2.BinCode
				, 1
				, T1.DistNumber
			from OBBQ T0 with(nolock)
				inner join OBTN T1 with(nolock) on T0.SnBMDAbs = T1.AbsEntry
				inner join OBIN T2 with(nolock) on T0.BinAbs = T2.AbsEntry
			where 1=1
				and T0.OnHandQty <> 0
				and T0.ItemCode = isnull(@ItemCode, T0.ItemCode)
				and T1.DistNumber = isnull(@BatchSerial, T1.DistNumber)
				and T0.BinAbs = isnull(@BinCode, T0.BinAbs)
			union all
			select
				T0.ItemCode
				, T0.WhsCode
				, T2.BinCode
				, T0.OnHandQty
				, T1.DistNumber
			from OSBQ T0 with(nolock)
				inner join OSRN T1 with(nolock) on T0.SnBMDAbs = T1.AbsEntry
				inner join OBIN T2 with(nolock) on T0.BinAbs = T2.AbsEntry
			where 1=1
				and T0.OnHandQty <> 0
				and T0.ItemCode = isnull(@ItemCode, T0.ItemCode)
				and T1.DistNumber = isnull(@BatchSerial, T1.DistNumber)
				and T0.BinAbs = isnull(@BinCode, T0.BinAbs);
	end;

	declare @cur1 as cursor 
	set @cur1 = cursor local fast_forward read_only for
		select ItemCode, DistNumber, OnHandQty from #tmp;

	open @cur1
	fetch next from @cur1 into @_itemCode, @_distNumber, @_onHandQty

	while @@fetch_status = 0
	begin
		set @i = 0;

		while @i < @_onHandQty
		begin
			insert into #results values(@_itemCode, @_distNumber);
			set @i = @i+1;
		end;

		fetch next from @cur1 into @_itemCode, @_distNumber, @_onHandQty
	end;

	close @cur1;
	deallocate @cur1;

	select * from #results;

	drop table #tmp;
	drop table #results;

	/*
	DECLARE @ItemManagement NVARCHAR(1)

	SELECT @ItemManagement =
	CASE WHEN ManBtchNum = 'Y'
		THEN 'B'
		ELSE 
			CASE WHEN ManSerNum = 'Y' THEN 'S'
			ELSE ''
		END
	END FROM OITM WHERE ItemCode = @ItemCode

	IF @ItemManagement = 'B'
	BEGIN
		SELECT DISTINCT
			OBTN.ItemCode,
			OBTN.DistNumber
		FROM OBTN WITH(NOLOCK)
			LEFT JOIN ITL1 WITH(NOLOCK)
				ON OBTN.AbsEntry = ITL1.MdAbsEntry
				AND OBTN.ItemCode = ITL1.ItemCode
			LEFT JOIN OBBQ WITH(NOLOCK)
				ON OBBQ.SnBMDAbs = OBTN.AbsEntry
		WHERE OBTN.ItemCode = ISNULL(@ItemCode, OBTN.ItemCode)
			AND OBTN.DistNumber = ISNULL(@BatchSerial, OBTN.DistNumber)
			AND OBBQ.BinAbs = ISNULL(@BinCode, OBBQ.BinAbs)
		GROUP BY OBTN.ItemCode, OBTN.DistNumber
		HAVING SUM(ITL1.Quantity) > 0
	END
	ELSE IF @ItemManagement = 'S'
	BEGIN
		SELECT
			OSRN.ItemCode,
			OSRN.DistNumber
		FROM OSRN WITH(NOLOCK)
			LEFT JOIN ITL1 WITH(NOLOCK)
				ON OSRN.AbsEntry = ITL1.MdAbsEntry
				AND OSRN.ItemCode = ITL1.ItemCode
			LEFT JOIN OSBQ WITH(NOLOCK)
				ON OSBQ.SnBMDAbs = OSRN.AbsEntry
		WHERE OSRN.ItemCode = ISNULL(@ItemCode, OSRN.ItemCode)
			AND OSRN.DistNumber = ISNULL(@BatchSerial, OSRN.DistNumber)
			AND OSBQ.BinAbs = ISNULL(@BinCode, OSBQ.BinAbs)
		GROUP BY OSRN.ItemCode, OSRN.DistNumber
		HAVING SUM(ITL1.Quantity) > 0
	END
	ELSE
	BEGIN
		SELECT DISTINCT
			OBTN.ItemCode,
			OBTN.DistNumber
		FROM OBTN WITH(NOLOCK)
			LEFT JOIN ITL1 WITH(NOLOCK)
				ON OBTN.AbsEntry = ITL1.MdAbsEntry
				AND OBTN.ItemCode = ITL1.ItemCode
			LEFT JOIN OBBQ WITH(NOLOCK)
				ON OBBQ.SnBMDAbs = OBTN.AbsEntry
		WHERE OBTN.ItemCode = ISNULL(@ItemCode, OBTN.ItemCode)
			AND OBTN.DistNumber = ISNULL(@BatchSerial, OBTN.DistNumber)
			AND OBBQ.BinAbs = ISNULL(@BinCode, OBBQ.BinAbs)
		GROUP BY OBTN.ItemCode, OBTN.DistNumber
		HAVING SUM(ITL1.Quantity) > 0

		UNION ALL

		SELECT
			OSRN.ItemCode,
			OSRN.DistNumber
		FROM OSRN WITH(NOLOCK)
			LEFT JOIN ITL1 WITH(NOLOCK)
				ON OSRN.AbsEntry = ITL1.MdAbsEntry
				AND OSRN.ItemCode = ITL1.ItemCode
			LEFT JOIN OSBQ WITH(NOLOCK)
				ON OSBQ.SnBMDAbs = OSRN.AbsEntry
		WHERE OSRN.ItemCode = ISNULL(@ItemCode, OSRN.ItemCode)
			AND OSRN.DistNumber = ISNULL(@BatchSerial, OSRN.DistNumber)
			AND OSBQ.BinAbs = ISNULL(@BinCode, OSBQ.BinAbs)
		GROUP BY OSRN.ItemCode, OSRN.DistNumber
		HAVING SUM(ITL1.Quantity) > 0
	END
	*/
END